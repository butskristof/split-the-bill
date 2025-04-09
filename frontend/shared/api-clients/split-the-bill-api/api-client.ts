import createClient from 'openapi-fetch';
import type { paths } from '~~/shared/api-clients/split-the-bill-api/spec';
import type {
  GetGroupResponse,
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

export class SplitTheBillApiClient {
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
  getGroup = (id: string): Promise<ApiResponse<GetGroupResponse>> =>
    this.client.GET('/Groups/{id}', {
      params: { path: { id } },
    });
}
