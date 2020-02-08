export abstract class RemoteDataState {
  public readonly isSuccess: boolean = false;
  public readonly isFailure: boolean = false;
  public readonly isLoading: boolean = false;
  public readonly isNotAsked: boolean = false;
  public readonly isUpdating: boolean = false;
  public readonly isUpdateFailure: boolean = false;
  public readonly isNotCreated: boolean = false;
}
