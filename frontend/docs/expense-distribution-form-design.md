# Expense Distribution Form Design

## Overview

This document describes the design approach for implementing expense distribution in the EditExpenseDialog component. The form supports three different modes for splitting expenses among participants:

1. **Evenly**: Split equally among all included participants
2. **Percentual**: Each participant covers a specific percentage
3. **Exact Amount**: Each participant covers a specific dollar amount

## Form State Structure

### Participant Form Data

Each participant in the form should have an enriched data structure:

```typescript
type ParticipantFormData = {
  member: Member;
  isIncluded: boolean;  // Controls the checkbox
  percentualShare: number | null;  // For Percentual mode
  exactShare: number | null;  // For Exact Amount mode
};

type FormState = {
  description: string;
  amount: number | null;
  timestamp: Date | null;
  paidBy: Member | null;
  splitType: ExpenseSplitType;  // NEW: Track which split mode
  participants: ParticipantFormData[];  // CHANGED: Richer structure
};
```

### Benefits of This Approach

- Single source of truth for all participant data
- All data available for realtime validation
- Easy to compute derived values (calculated amounts, totals, etc.)
- Clear TypeScript types
- Smoother transitions when switching between split modes

## Validation Strategy with Regle

### Conditional Validation Based on Split Type

Validation rules should be conditional based on the active `splitType`. Regle supports this through computed schemas:

```typescript
const formSchema = computed(() => {
  const splitType = formState.value.splitType;

  return inferRules(formState, {
    // ... other fields ...

    splitType: { required },

    participants: {
      required,
      $each: {
        member: { /* validate member */ },
        isIncluded: { /* boolean */ },

        // Conditional validation based on splitType
        percentualShare: splitType === ExpenseSplitType.Percentual
          ? {
              required: requiredIf(() => isIncluded),
              minValue: minValue(0),
              maxValue: maxValue(100)
            }
          : {},  // No validation for other modes

        exactShare: splitType === ExpenseSplitType.ExactAmount
          ? {
              required: requiredIf(() => isIncluded),
              minValue: minValue(0)
            }
          : {},  // No validation for other modes
      }
    }
  });
});
```

### Cross-Field Validation

Validation rules that span multiple participants:

#### For Percentual Mode
- Sum of percentages should equal 100% (within tolerance)
- Only included participants count toward the total

#### For Exact Amount Mode
- Sum of exact shares should equal total expense amount
- Only included participants count toward the total

#### For Evenly Mode
- At least one participant must be included

#### Custom Validator Example

```typescript
// Custom validator for percentage totals
const percentagesAddUpTo100 = () => {
  return (value, { $value }) => {
    if ($value.splitType !== ExpenseSplitType.Percentual) return true;

    const includedParticipants = $value.participants.filter(p => p.isIncluded);
    const total = includedParticipants.reduce((sum, p) => sum + (p.percentualShare || 0), 0);

    return Math.abs(total - 100) < 0.01;  // Allow small floating point errors
  };
};
```

## Computed Display Values

### Participant Display Amounts

Calculate individual amounts based on split type for display purposes:

```typescript
// Calculate individual amounts based on split type
const participantDisplayAmounts = computed(() => {
  const { amount, splitType, participants } = formState.value;
  if (!amount) return new Map();

  return participants.map(p => {
    if (!p.isIncluded) return { memberId: p.member.id, amount: 0 };

    switch (splitType) {
      case ExpenseSplitType.Evenly:
        const includedCount = participants.filter(x => x.isIncluded).length;
        return { memberId: p.member.id, amount: amount / includedCount };

      case ExpenseSplitType.Percentual:
        return {
          memberId: p.member.id,
          amount: p.percentualShare ? (amount * p.percentualShare / 100) : 0
        };

      case ExpenseSplitType.ExactAmount:
        return { memberId: p.member.id, amount: p.exactShare || 0 };
    }
  });
});
```

### Total Distributed Amount

```typescript
// Calculate total distributed amount
const totalDistributed = computed(() => {
  return participantDisplayAmounts.value
    .reduce((sum, p) => sum + p.amount, 0);
});
```

### Progress Bar Value

```typescript
// Progress bar value (0-100)
const distributionProgress = computed(() => {
  const total = formState.value.amount;
  if (!total) return 0;
  return Math.min(100, (totalDistributed.value / total) * 100);
});
```

## UI Rendering Logic

Conditionally render participant rows based on split type:

```vue
<template v-if="formState.splitType === ExpenseSplitType.Evenly">
  <!-- Checkbox + readonly amount label only -->
</template>

<template v-else-if="formState.splitType === ExpenseSplitType.Percentual">
  <!-- Checkbox + percentage input + calculated amount label -->
</template>

<template v-else-if="formState.splitType === ExpenseSplitType.ExactAmount">
  <!-- Checkbox + amount input only -->
</template>
```

### Display Components Per Mode

| Split Type | Checkbox | Input Field | Amount Label |
|------------|----------|-------------|--------------|
| Evenly | ✓ | ✗ | ✓ (readonly, calculated) |
| Percentual | ✓ | ✓ (percentage) | ✓ (readonly, calculated) |
| Exact Amount | ✓ | ✓ (amount) | ✗ |

## API Request Mapping

When submitting, filter and map based on split type:

```typescript
const buildApiRequest = () => {
  const includedParticipants = formState.value.participants
    .filter(p => p.isIncluded);

  return {
    // ... other fields ...
    splitType: formState.value.splitType,
    participants: includedParticipants.map(p => {
      const base = { memberId: p.member.id };

      switch (formState.value.splitType) {
        case ExpenseSplitType.Evenly:
          return base;  // No additional fields

        case ExpenseSplitType.Percentual:
          return { ...base, percentualShare: p.percentualShare! };

        case ExpenseSplitType.ExactAmount:
          return { ...base, exactShare: p.exactShare! };
      }
    })
  };
};
```

## Key Design Decisions

1. **Keep all data in form state**: Even fields that aren't currently relevant (e.g., percentualShare when in Evenly mode). This makes switching between modes smoother and keeps validation simple.

2. **Conditional Regle schemas**: Use computed schemas that change based on splitType to enable/disable validation rules dynamically.

3. **Separate display logic from state**: Use computed properties for derived/display values rather than storing them in state.

4. **Type safety**: TypeScript will help ensure you handle all split types in switch statements.

5. **Initialize wisely**: When creating the form, pre-populate participants array with all members, set reasonable defaults (isIncluded: true, shares: null).

## Open Questions to Consider

### 1. Mode Switching
If the user changes `splitType` mid-entry, should you:
- **Preserve** the percentualShare/exactShare values (allow switching back without data loss)?
- **Clear** the values (clean slate for new mode)?

### 2. Default Behavior
When creating a new expense, what should the default splitType be?
- Evenly (most common use case)?
- User's last used setting (requires persistence)?

### 3. Validation Timing
Should validation be:
- **Aggressive**: Show errors immediately as user types
- **Lazy**: Only show errors on blur or submit
- **Hybrid**: Immediate for some fields, lazy for others

### 4. Tolerance for Floating Point Errors
For percentages totaling 100%, what tolerance is acceptable?
- Strict: Exactly 100.00%
- Lenient: 99.99% - 100.01% (account for floating point arithmetic)

### 5. Empty State Handling
What happens when:
- No participants are selected (at least one required)?
- Amount is zero or empty?
- User unchecks all participants?

## Implementation Checklist

- [ ] Add `splitType` field to form state
- [ ] Update `ParticipantFormData` type with new fields
- [ ] Create computed schema with conditional validation
- [ ] Implement custom cross-field validators
- [ ] Create computed properties for display amounts
- [ ] Update UI to conditionally render based on split type
- [ ] Add split type selector (radio buttons or dropdown)
- [ ] Update progress bar to show distribution
- [ ] Update API request builder
- [ ] Handle initialization from existing expense data
- [ ] Add unit tests for validation logic
- [ ] Test mode switching behavior
- [ ] Test edge cases (zero amounts, all unchecked, etc.)

## Related Files

- `/frontend/app/components/groups/detail/EditExpenseDialog.vue` - Main form component
- `/frontend/shared/types/api.ts` - API request types
- `/frontend/shared/types/expense-split-type.ts` - Split type enum
