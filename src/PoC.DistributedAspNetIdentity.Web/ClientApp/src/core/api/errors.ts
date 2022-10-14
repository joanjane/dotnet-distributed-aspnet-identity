export type ApiErrorResponse = {
  status: number;
  errors: Record<string, string[]>;
}

export enum ApiErrorCode {
  InvalidCredentials = 'InvalidCredentials'
};

export class HttpError extends Error {
  public errorCodes: string[];
  public statusCode: number;

  constructor(statusCode: number, errors: ApiErrorResponse, message?: string) {
    super(message ?? `Http error ${errors.status}`);
    this.name = 'ApiError';
    this.statusCode = statusCode;
    this.errorCodes = errors?.errors ? Object.keys(errors.errors) : [];

    if (this.errorCodes.length === 0) {
      throw new Error(`Invalid response ${errors.status}, no error codes found.`);
    }
  }

  containsError(error: ApiErrorCode): boolean {
    return this.errorCodes.some(e => e === error);
  }
}

export class UnknownHttpError extends Error {
  constructor(response: Response, message?: string) {
    super(`Request ${response.url} failed with status ${response.status}. ${message}`);
    this.name = 'ApiError';
  }
}
