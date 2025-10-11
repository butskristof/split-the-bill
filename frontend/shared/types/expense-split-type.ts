export const ExpenseSplitType = {
  Evenly: 0,
  Percentual: 1,
  ExactAmount: 2,
} as const;

export type ExpenseSplitType = (typeof ExpenseSplitType)[keyof typeof ExpenseSplitType];
