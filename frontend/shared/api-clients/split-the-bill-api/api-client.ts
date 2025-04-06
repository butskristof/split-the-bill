import createClient from 'openapi-fetch';
import type { paths } from '~~/shared/api-clients/split-the-bill-api/spec';
import type {
  GetGroupsResponse,
  GetMembersResponse,
  ProblemDetails,
} from '#shared/api-clients/split-the-bill-api/types';
import type { ReturnType } from 'birpc';

export type ApiResponse<T> = {
  response?: Response;
  data?: T;
  error?: ProblemDetails;
};

export class SplitTeBillApiClient {
  private client: ReturnType<typeof createClient<paths>>;

  constructor(baseUrl: string, accessTokenGetter: () => string | null) {
    const client = createClient<paths>({ baseUrl });
    client.use({
      onRequest: ({ request }) => {
        const accessToken = accessTokenGetter();
        if (accessToken) request.headers.set('Authorization', `Bearer ${accessToken}`);
        return request;
      },
    });

    this.client = client;
  }

  getMembers = (): Promise<ApiResponse<GetMembersResponse>> => this.client.GET('/Members');
  getGroups = (): Promise<ApiResponse<GetGroupsResponse>> => this.client.GET('/Groups');
}

// const createSplitTheBillApiClient = () => createClient<paths>({ baseUrl: 'http://localhost:5222' });
//
// export const getMembers = (): Promise<ApiResponse<GetMembersResponse>> =>
//   createSplitTheBillApiClient().GET('/Members');
//
// export const getGroups = () => createSplitTheBillApiClient().GET('/Groups');
//
// export const getGroup = (id: string) =>
//   createSplitTheBillApiClient().GET('/Groups/{id}', {
//     params: { path: { id } },
//   });
//
// export const postGroup = (request: components['schemas']['CreateGroup.Request']) =>
//   createSplitTheBillApiClient().POST('/Groups', {
//     body: request,
//   });
//
// export const putGroup = (id: string, request: components['schemas']['UpdateGroup.Request']) =>
//   createSplitTheBillApiClient().PUT('/Groups/{id}', {
//     params: { path: { id } },
//     body: request,
//   });
//
// export const deleteGroup = (id: string) =>
//   createSplitTheBillApiClient().DELETE('/Groups/{id}', {
//     params: { path: { id } },
//   });
