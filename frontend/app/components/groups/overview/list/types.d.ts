import type { BackendApiResponse } from '#open-fetch';

export type Group = BackendApiResponse<'GetGroups'>['groups'][0];
