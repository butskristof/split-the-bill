import type { BackendApiResponse } from '#open-fetch';

export type Group = BackendApiResponse<'GetGroup'>;
export type Expense = Group['expenses'][0] & { type: 'Expense' };
export type Payment = Group['payments'][0] & { type: 'Payment' };
export type Activity = Expense | Payment;
export type GroupMember = Group['members'][0];
