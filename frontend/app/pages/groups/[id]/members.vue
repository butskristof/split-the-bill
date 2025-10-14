<template>
  <div class="page-group-detail-members">
    <AppBackButton
      label="Back to group"
      :to="{ name: 'groups-id', params: { id: groupId } }"
    />
    <h2>Members</h2>
    <Accordion
      multiple
      :value="accordionValue"
    >
      <AccordionPanel
        v-for="member in members"
        :key="member.id"
        :value="member.id"
      >
        <AccordionHeader>
          <InlineGroupMember
            size="normal"
            :member="{ id: member.id!, name: member.name! }"
          />
          <div class="balance">
            <span>{{ member.totalBalance! < 0 ? 'owes' : 'is owed' }}&nbsp;</span>
            <strong
              :class="{
                positive: member.totalBalance! >= 0,
                negative: member.totalBalance! < 0,
              }"
              >{{ formatCurrency(Math.abs(member.totalBalance!)) }}</strong
            >
          </div>
        </AccordionHeader>
        <AccordionContent>
          <div class="member-details">
            <div class="row">
              <div class="label">Total expense amount</div>
              <div class="value">{{ formatCurrency(member.totalExpenseAmount!) }}</div>
            </div>
            <div class="row">
              <div class="label">Total expense paid amount</div>
              <div class="value">{{ formatCurrency(member.totalExpensePaidAmount!) }}</div>
            </div>
            <div class="row">
              <div class="label">Total expenses paid by other members</div>
              <div class="value">
                {{ formatCurrency(member.totalExpenseAmountPaidByOtherMembers!) }}
              </div>
            </div>
            <div class="row">
              <div class="label">Total payment received</div>
              <div class="value">{{ formatCurrency(member.totalPaymentReceivedAmount!) }}</div>
            </div>
            <div class="row">
              <div class="label">Total payment sent</div>
              <div class="value">{{ formatCurrency(member.totalPaymentSentAmount!) }}</div>
            </div>
            <div class="row">
              <div class="label">Total amount owed</div>
              <div class="value">{{ formatCurrency(member.totalAmountOwed!) }}</div>
            </div>
            <div class="row">
              <div class="label">Total amount owed to other members</div>
              <div class="value">
                {{ formatCurrency(member.totalExpenseAmountPaidByOtherMembers!) }}
              </div>
            </div>
            <div class="row">
              <div class="label"><strong>Balance</strong></div>
              <div class="value">
                <span>{{ member.totalBalance! < 0 ? 'owes' : 'is owed' }}&nbsp;</span>
                <strong
                  :class="{
                    positive: member.totalBalance! >= 0,
                    negative: member.totalBalance! < 0,
                  }"
                  >{{ formatCurrency(Math.abs(member.totalBalance!)) }}</strong
                >
              </div>
            </div>
          </div>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
  </div>
</template>

<script setup lang="ts">
import AppBackButton from '~/components/app/AppBackButton.vue';
import { useDetailPageGroup } from '~/composables/backend-api/useDetailPageGroup';
import InlineGroupMember from '~/components/common/InlineGroupMember.vue';
import { formatCurrency } from '#shared/utils';

const { groupId, members } = useDetailPageGroup();
const accordionValue = computed(() => (members.value?.length ? [members.value[0]!.id!] : []));
</script>

<style scoped lang="scss">
@use '~/styles/_utilities.scss';
.page-group-detail-members {
  @include utilities.flex-column;
}

:deep(.p-accordionheader) {
  .balance {
    margin-left: auto;
    margin-right: var(--default-spacing);
  }
}

:deep(.p-accordioncontent-content) {
  @include utilities.flex-column;
}

.member-details {
  @include utilities.flex-column;

  .row {
    @include utilities.flex-row-justify-between;
    flex-wrap: wrap;
    .value {
      margin-left: auto;
    }
  }
}

.positive {
  color: var(--p-green-600);
}

.negative {
  color: var(--p-red-600);
}
</style>
