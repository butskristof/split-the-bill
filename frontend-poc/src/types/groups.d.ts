export type GroupDto = {
  id: string;
  name: string;
  members: MemberDto[];
  expenses: ExpenseDto[];
  payments: PaymentDto[];
  totalExpenseAmount: number;
  totalPaymentAmount: number;
  totalBalance: number;
};

type MemberDto = {
  id: string;
  name: string;
  totalExpenseAmount: number;
  totalExpensePaidAmount: number;
  totalExpenseAmountPaidByOtherMembers: number;
  totalPaymentReceivedAmount: number;
  totalPaymentSentAmount: number;
  totalAmountOwed: number;
  totalAmountOwedToOtherMembers: number;
  totalBalance: number;
};

type ExpenseDto = {
  id: string;
  description: string;
  amount: number;
  splitType: ExpenseSplitType;
  participants: ExpenseParticipantDto[];
  paidByMemberId: string;
};

type ExpenseParticipantDto = {
  memberId: string;
  percentualShare: number | null;
  exactShare: number | null;
};

type PaymentDto = {
  id: string;
  sendingMemberId: string;
  receivingMemberId: string;
  amount: number;
};

enum ExpenseSplitType {
  Equal = 0,
  Percentual = 1,
  Exact = 2,
}
