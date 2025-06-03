import type { FetchError } from 'ofetch';
import type { _AsyncData } from '#app/composables/asyncData';
import type { components } from '#open-fetch-schemas/backend-api';

export type Query<T> = _AsyncData<T | null, FetchError | null>;
export type ProblemDetails = components['schemas']['ProblemDetails'];
export type ValidationProblemDetails = components['schemas']['HttpValidationProblemDetails'];
