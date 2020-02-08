import { IDataWrapper } from './IDataWrapper';
import { IErrorWrapper } from './IErrorWrapper';
import { IMatchCases } from './IMatchCases';
import { IMonad } from './IMonad';
import { RemoteDataState } from './RdSates/RemoteDataState';
import { NotAsked } from './RdSates/NotAsked';
import { Loading } from './RdSates/Loading';
import { Failure } from './RdSates/Failure';
import { Success } from './RdSates/Success';
import { Updating } from './RdSates/Updating';
import { UpdateFailure } from './RdSates/UpdateFailure';
import { NotCreated } from './RdSates/NotCreated';

interface IRemoteDataMatchCases<T, E> extends IMatchCases {
  successFunc: (data: T) => any;
  failureFunc: (error: E) => any;
  loadingFunc: () => any;
  notAskedFunc: () => any;
  updatingFunc: (data: T) => any;
  updateFailureFunc: (data: T, error: E) => any;
  notCreatedFunc: (data: T) => any;
}

export class UpdatableRemoteData<T, E> implements IMonad<T>, IDataWrapper<T>, IErrorWrapper<E> {
  /**
   * Construct a new Remote Data that was local data only for now
   *
   * @param data The local data that will be created remotely
   */
  public static notCreated<T, E>(data: T): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new NotCreated<T>(data));
  }

  /**
   * Construct a new Remote Data that failed during update or creation
   *
   * @param data The data present before the update
   * @param error The error that occurred during update
   */
  public static updateFailure<T, E>(data: T, error: E): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new UpdateFailure<T, E>(data, error));
  }

  /**
   * Construct a new Remote Data that is updating or creating remotely
   *
   * @param data The data that was loaded previously
   */
  public static updating<T, E>(data: T): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new Updating<T>(data));
  }

  /**
   * Construct a new Remote Data that succeeded
   *
   * @param data The data that was loaded
   */
  public static success<T, E>(data: T): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new Success<T>(data));
  }

  /**
   * Construct a new Remote Data that failed loading
   *
   * @param error The error that occurred
   */
  public static failure<T, E>(error: E): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new Failure<E>(error));
  }

  /**
   * Construct a new Remote Data that is loading
   */
  public static loading<T, E>(): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new Loading());
  }

  /**
   * Construct a new Remote Data that was not asked for yet
   */
  public static notAsked<T, E>(): UpdatableRemoteData<T, E> {
    return new UpdatableRemoteData<T, E>(new NotAsked());
  }

  private state: RemoteDataState;

  private constructor(state: RemoteDataState) {
    this.state = state;
  }

  public match({
    successFunc,
    failureFunc,
    loadingFunc,
    notAskedFunc,
    updatingFunc,
    updateFailureFunc,
    notCreatedFunc
  }: IRemoteDataMatchCases<T, E>): any {
    if (this.state.isSuccess) {
      return successFunc((this.state as Success<T>).data);
    }
    if (this.state.isFailure) {
      return failureFunc((this.state as Failure<E>).error);
    }
    if (this.state.isLoading) {
      return loadingFunc();
    }
    if (this.state.isNotAsked) {
      return notAskedFunc();
    }
    if (this.state.isUpdating) {
      return updatingFunc((this.state as Updating<T>).data);
    }
    if (this.state.isUpdateFailure) {
      return updateFailureFunc(
        (this.state as UpdateFailure<T, E>).data,
        (this.state as UpdateFailure<T, E>).error
      );
    }
    if (this.state.isNotCreated) {
      return notCreatedFunc((this.state as NotCreated<T>).data);
    }
    throw new Error('Unmatched case in RemoteData');
  }

  public map<R>(func: (value: T) => R): UpdatableRemoteData<R, E> {
    return this.match({
      successFunc: data => UpdatableRemoteData.success<R, E>(func(data)),
      failureFunc: UpdatableRemoteData.failure,
      loadingFunc: UpdatableRemoteData.loading,
      notAskedFunc: UpdatableRemoteData.notAsked,
      updatingFunc: data => UpdatableRemoteData.updating<R, E>(func(data)),
      updateFailureFunc: (data, error) =>
        UpdatableRemoteData.updateFailure<R, E>(func(data), error),
      notCreatedFunc: data => UpdatableRemoteData.notCreated<R, E>(func(data))
    } as IRemoteDataMatchCases<T, E>);
  }

  public mapError<F>(func: (error: E) => F): UpdatableRemoteData<T, F> {
    return this.match({
      successFunc: UpdatableRemoteData.success,
      failureFunc: error => UpdatableRemoteData.failure<T, F>(func(error)),
      loadingFunc: UpdatableRemoteData.loading,
      notAskedFunc: UpdatableRemoteData.notAsked,
      updatingFunc: UpdatableRemoteData.updating,
      updateFailureFunc: (data, error) =>
        UpdatableRemoteData.updateFailure<T, F>(data, func(error)),
      notCreatedFunc: UpdatableRemoteData.notCreated
    } as IRemoteDataMatchCases<T, E>);
  }

  public andThen<R>(func: (value: T) => UpdatableRemoteData<R, E>): UpdatableRemoteData<R, E> {
    return this.match({
      successFunc: func,
      failureFunc: error => UpdatableRemoteData.failure<T, E>(error),
      loadingFunc: UpdatableRemoteData.loading,
      notAskedFunc: UpdatableRemoteData.notAsked,
      updatingFunc: func,
      updateFailureFunc: (data, error) => func(data),
      notCreatedFunc: func
    } as IRemoteDataMatchCases<T, E>);
  }

  public withDefault(defaultValue: T): T {
    return this.hasData() ? this.getData() : defaultValue;
  }
  public isNotCreated = () => this.state.isNotCreated;
  public isUpdateFailure = () => this.state.isUpdateFailure;
  public isUpdating = () => this.state.isUpdating;
  public isSuccess = () => this.state.isSuccess;
  public isFailure = () => this.state.isFailure;
  public isLoading = () => this.state.isLoading;
  public isNotAsked = () => this.state.isNotAsked;
  public hasData = () =>
    this.isSuccess() || this.isUpdateFailure() || this.isNotCreated() || this.isUpdating();
  public hasError = () => this.isFailure() || this.isUpdateFailure();

  public getData(): T {
    if (this.hasData()) {
      return (this.state as Success<T>).data;
    }
    throw new Error('RemoteData is not in a data carrying state');
  }

  public getError(): E {
    if (this.hasError()) {
      return (this.state as Failure<E>).error;
    }
    throw new Error('RemoteData is not in a failure state');
  }
}
