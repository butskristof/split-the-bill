// Re-export from OpenAPI with convenient shorthand names
// shared/types is auto-imported so much easier to use throughout the application rather
// than importing the components over and over again
import type { components } from '#open-fetch-schemas/backend-api';
import type { ExpenseSplitType } from './expense-split-type';

export type ValidationProblemDetails = components['schemas']['HttpValidationProblemDetails'];
export type ProblemDetails = components['schemas']['ProblemDetails'];

// adaption of components['schemas']['XXX.Request'] which removed nullables where we know
// them to be required (technically allowed in DTO but will be rejected by server-side validation)
export type CreateGroupRequest = {
  name: string;
};
export type CreateExpenseRequest = {
  groupId: string;
  description: string;
  amount: number;
  timestamp: string;
  paidByMemberId: string;
  splitType: ExpenseSplitType;
  participants: {
    memberId: string;
    percentualShare?: number;
    exactShare?: number;
  }[];
};
export type CreatePaymentRequest = {
  groupId: string;
  sendingMemberId: string;
  receivingMemberId: string;
  amount: number;
  timestamp: string;
};

export type Group = components['schemas']['GroupDto'];
