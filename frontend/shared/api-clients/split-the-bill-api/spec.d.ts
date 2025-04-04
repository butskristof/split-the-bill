/**
 * This file was auto-generated by openapi-typescript.
 * Do not make direct changes to the file.
 */

export interface paths {
    "/Members": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: operations["GetMembers"];
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/Groups": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: operations["GetGroups"];
        put?: never;
        post: operations["CreateGroup"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/Groups/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: operations["GetGroup"];
        put: operations["UpdateGroup"];
        post?: never;
        delete: operations["DeleteGroup"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/Groups/{groupId}/expenses": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        post: operations["CreateExpense"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/Groups/{groupId}/expenses/{expenseId}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put: operations["UpdateExpense"];
        post?: never;
        delete: operations["DeleteExpense"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/Groups/{groupId}/payments": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        post: operations["CreatePayment"];
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/Groups/{groupId}/payments/{paymentId}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put: operations["UpdatePayment"];
        post?: never;
        delete: operations["DeletePayment"];
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
}
export type webhooks = Record<string, never>;
export interface components {
    schemas: {
        /** CreateExpense.Request */
        "CreateExpense.Request": {
            /** Format: uuid */
            groupId?: string | null;
            description?: string | null;
            /** Format: uuid */
            paidByMemberId?: string | null;
            /** Format: date-time */
            timestamp?: string | null;
            /** Format: double */
            amount?: number | null;
            splitType?: components["schemas"]["NullableOfExpenseSplitType"];
            participants?: components["schemas"]["CreateExpense.Request.Participant"][];
        };
        /** CreateExpense.Request.Participant */
        "CreateExpense.Request.Participant": {
            /** Format: uuid */
            memberId?: string | null;
            /** Format: int32 */
            percentualShare?: number | null;
            /** Format: double */
            exactShare?: number | null;
        };
        /** CreateGroup.Request */
        "CreateGroup.Request": {
            name?: string | null;
        };
        /** CreateGroup.Response */
        "CreateGroup.Response": {
            /** Format: uuid */
            id: string;
        };
        /** CreatePayment.Request */
        "CreatePayment.Request": {
            /** Format: uuid */
            groupId?: string | null;
            /** Format: uuid */
            sendingMemberId?: string | null;
            /** Format: uuid */
            receivingMemberId?: string | null;
            /** Format: double */
            amount?: number | null;
            /** Format: date-time */
            timestamp?: string | null;
        };
        ExpenseSplitType: number;
        /** GetGroups.Response */
        "GetGroups.Response": {
            groups: components["schemas"]["GetGroups.Response.GroupDto"][];
        };
        /** GetGroups.Response.GroupDto */
        "GetGroups.Response.GroupDto": {
            /** Format: uuid */
            id: string;
            name: string;
        };
        /** GetMembers.Response */
        "GetMembers.Response": {
            members: components["schemas"]["GetMembers.Response.MemberDto"][];
        };
        /** GetMembers.Response.MemberDto */
        "GetMembers.Response.MemberDto": {
            /** Format: uuid */
            id: string;
            name: string;
        };
        /** GroupDto */
        GroupDto: {
            /** Format: uuid */
            id?: string;
            name?: string | null;
            members?: components["schemas"]["GroupDto.MemberDto"][] | null;
            expenses?: components["schemas"]["GroupDto.ExpenseDto"][] | null;
            payments?: components["schemas"]["GroupDto.PaymentDto"][] | null;
            /** Format: double */
            totalExpenseAmount?: number;
            /** Format: double */
            totalPaymentAmount?: number;
            /** Format: double */
            totalBalance?: number;
        };
        /** GroupDto.ExpenseDto */
        "GroupDto.ExpenseDto": {
            /** Format: uuid */
            id?: string;
            description?: string;
            /** Format: uuid */
            paidByMemberId?: string;
            /** Format: date-time */
            timestamp?: string;
            /** Format: double */
            amount?: number;
            splitType?: components["schemas"]["ExpenseSplitType"];
            participants?: components["schemas"]["GroupDto.ExpenseDto.ExpenseParticipantDto"][];
        };
        /** GroupDto.ExpenseDto.ExpenseParticipantDto */
        "GroupDto.ExpenseDto.ExpenseParticipantDto": {
            /** Format: uuid */
            memberId?: string;
            /** Format: int32 */
            percentualShare?: number | null;
            /** Format: double */
            exactShare?: number | null;
        };
        /** GroupDto.MemberDto */
        "GroupDto.MemberDto": {
            /** Format: uuid */
            id?: string;
            name?: string | null;
            /** Format: double */
            totalExpenseAmount?: number;
            /** Format: double */
            totalExpensePaidAmount?: number;
            /** Format: double */
            totalExpenseAmountPaidByOtherMembers?: number;
            /** Format: double */
            totalPaymentReceivedAmount?: number;
            /** Format: double */
            totalPaymentSentAmount?: number;
            /** Format: double */
            totalAmountOwed?: number;
            /** Format: double */
            totalAmountOwedToOtherMembers?: number;
            /** Format: double */
            totalBalance?: number;
        };
        /** GroupDto.PaymentDto */
        "GroupDto.PaymentDto": {
            /** Format: uuid */
            id?: string;
            /** Format: uuid */
            sendingMemberId?: string;
            /** Format: uuid */
            receivingMemberId?: string;
            /** Format: double */
            amount?: number;
            /** Format: date-time */
            timestamp?: string;
        };
        /** HttpValidationProblemDetails */
        HttpValidationProblemDetails: {
            type?: string | null;
            title?: string | null;
            /** Format: int32 */
            status?: number | null;
            detail?: string | null;
            instance?: string | null;
            errors?: {
                [key: string]: string[];
            };
        };
        NullableOfExpenseSplitType: number | null;
        /** ProblemDetails */
        ProblemDetails: {
            type?: string | null;
            title?: string | null;
            /** Format: int32 */
            status?: number | null;
            detail?: string | null;
            instance?: string | null;
        };
        /** UpdateExpense.Request */
        "UpdateExpense.Request": {
            /** Format: uuid */
            groupId?: string | null;
            /** Format: uuid */
            expenseId?: string | null;
            description?: string | null;
            /** Format: uuid */
            paidByMemberId?: string | null;
            /** Format: date-time */
            timestamp?: string | null;
            /** Format: double */
            amount?: number | null;
            splitType?: components["schemas"]["NullableOfExpenseSplitType"];
            participants?: components["schemas"]["UpdateExpense.Request.Participant"][];
        };
        /** UpdateExpense.Request.Participant */
        "UpdateExpense.Request.Participant": {
            /** Format: uuid */
            memberId?: string | null;
            /** Format: int32 */
            percentualShare?: number | null;
            /** Format: double */
            exactShare?: number | null;
        };
        /** UpdateGroup.Request */
        "UpdateGroup.Request": {
            /** Format: uuid */
            id?: string;
            name?: string | null;
        };
        /** UpdatePayment.Request */
        "UpdatePayment.Request": {
            /** Format: uuid */
            groupId?: string | null;
            /** Format: uuid */
            paymentId?: string | null;
            /** Format: uuid */
            sendingMemberId?: string | null;
            /** Format: uuid */
            receivingMemberId?: string | null;
            /** Format: double */
            amount?: number | null;
            /** Format: date-time */
            timestamp?: string | null;
        };
    };
    responses: never;
    parameters: never;
    requestBodies: never;
    headers: never;
    pathItems: never;
}
export type $defs = Record<string, never>;
export interface operations {
    GetMembers: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GetMembers.Response"];
                };
            };
        };
    };
    GetGroups: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GetGroups.Response"];
                };
            };
        };
    };
    CreateGroup: {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/json": components["schemas"]["CreateGroup.Request"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["CreateGroup.Response"];
                };
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
        };
    };
    GetGroup: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description OK */
            200: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/json": components["schemas"]["GroupDto"];
                };
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    UpdateGroup: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/json": components["schemas"]["UpdateGroup.Request"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    DeleteGroup: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                id: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    CreateExpense: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                groupId: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/json": components["schemas"]["CreateExpense.Request"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    UpdateExpense: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                groupId: string;
                expenseId: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/json": components["schemas"]["UpdateExpense.Request"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    DeleteExpense: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                groupId: string;
                expenseId: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    CreatePayment: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                groupId: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/json": components["schemas"]["CreatePayment.Request"];
            };
        };
        responses: {
            /** @description Created */
            201: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    UpdatePayment: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                groupId: string;
                paymentId: string;
            };
            cookie?: never;
        };
        requestBody: {
            content: {
                "application/json": components["schemas"]["UpdatePayment.Request"];
            };
        };
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
    DeletePayment: {
        parameters: {
            query?: never;
            header?: never;
            path: {
                groupId: string;
                paymentId: string;
            };
            cookie?: never;
        };
        requestBody?: never;
        responses: {
            /** @description No Content */
            204: {
                headers: {
                    [name: string]: unknown;
                };
                content?: never;
            };
            /** @description Bad Request */
            400: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["HttpValidationProblemDetails"];
                };
            };
            /** @description Not Found */
            404: {
                headers: {
                    [name: string]: unknown;
                };
                content: {
                    "application/problem+json": components["schemas"]["ProblemDetails"];
                };
            };
        };
    };
}
