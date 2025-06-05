import type { FetchError } from 'ofetch';
import type { _AsyncData } from '#app/composables/asyncData';
import type { components } from '#open-fetch-schemas/backend-api';
import type { BackendApiResponse } from '#open-fetch';

export type Query<T> = _AsyncData<T | null, FetchError | null>;
export type ProblemDetails = components['schemas']['ProblemDetails'];
export type ValidationProblemDetails = components['schemas']['HttpValidationProblemDetails'];

export type Group = BackendApiResponse<'GetGroup'>;
export type Expense = Group['expenses'][0] & { type: 'Expense' };
export type Payment = Group['payments'][0] & { type: 'Payment' };
export type Activity = Expense | Payment;
export type GroupMember = Group['members'][0];
