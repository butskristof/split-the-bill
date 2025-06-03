import type { FetchError } from 'ofetch';
import type { _AsyncData } from '#app/composables/asyncData';

export type Query<T> = _AsyncData<T | null, FetchError | null>;
