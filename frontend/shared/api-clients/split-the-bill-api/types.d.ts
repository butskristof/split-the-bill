export type ProblemDetails = components['schemas']['ProblemDetails'];
export type GetMembersResponse = components['schemas']['GetMembers.Response'];
export type GetGroupsResponse = components['schemas']['GetGroups.Response'];
export type GetGroupResponse = components['schemas']['GroupDto'];

export type Member = {
  id: string;
  name: string;
};

export type GroupMember = Member & {
  totalExpenseAmount: number;
  totalExpensePaidAmount: number;
  totalExpenseAmountPaidByOtherMembers: number;
  totalPaymentReceivedAmount: number;
  totalPaymentSentAmount: number;
  totalAmountOwed: number;
  totalAmountOwedToOtherMembers: number;
  totalBalance: number;
};

export type Expense = {
  id: string;
  description: string;
  paidByMemberId: string;
  timestamp: Date;
  amount: number;
  splitType: ExpenseSplitType;
  participants: ExpenseParticipant[];
};

export type ExpenseParticipant =
  | {
      memberId: string;
    }
  | {
      memberId: string;
      percentualShare: number;
    }
  | {
      memberId: string;
      exactShare: number;
    };

export enum ExpenseSplitType {
  Evenly = 0,
  Percentual = 1,
  ExactAmount = 2,
}

export type Payment = {
  id: string;
  sendingMemberId: string;
  receivingMemberId: string;
  amount: number;
  timestamp: Date;
};

export type Group = {
  id: string;
  name: string;

  members: GroupMember[];
  expenses: Expense[];
  payments: Payment[];

  totalExpenseAmount: number;
  totalPaymentAmount: number;
  totalBalance: number;
};
