import { IDataWrapper } from './IDataWrapper';
import { IErrorWrapper } from './IErrorWrapper';
import { IMatchCases } from './IMatchCases';
import { IMonad } from './IMonad';

abstract class RemoteDataState {
  public readonly isSuccess: boolean = false;
  public readonly isFailure: boolean = false;
  public readonly isLoading: boolean = false;
  public readonly isNotAsked: boolean = false;
}

// tslint:disable-next-line: max-classes-per-file
class NotAsked extends RemoteDataState {
  public readonly isNotAsked: boolean = true;
}

// tslint:disable-next-line: max-classes-per-file
class Loading extends RemoteDataState {
  public readonly isLoading: boolean = true;
}

// tslint:disable-next-line: max-classes-per-file
class Failure<E> extends RemoteDataState {
  public readonly isFailure: boolean = true;
  public readonly error!: E;

  constructor(error: E) {
    super();
    this.error = error;
  }
}

// tslint:disable-next-line: max-classes-per-file
class Success<T> extends RemoteDataState {
  public readonly isSuccess: boolean = true;
  public readonly data!: T;

  constructor(data: T) {
    super();
    this.data = data;
  }
}

interface IRemoteDataMatchCases<T, E> extends IMatchCases {
  successFunc: (data: T) => any;
  failureFunc: (error: E) => any;
  loadingFunc: () => any;
  notAskedFunc: () => any;
}

// tslint:disable-next-line: max-classes-per-file
export class RemoteData<T, E> implements IMonad<T>, IDataWrapper<T>, IErrorWrapper<E> {
  /**
   * Construct a new Remote Data that succeeded
   *
   * @param data The data that was loaded
   */
  public static success<T, E>(data: T): RemoteData<T, E> {
    return new RemoteData<T, E>(new Success<T>(data));
  }

  /**
   * Construct a new Remote Data that failed loading
   *
   * @param error The error that occurred
   */
  public static failure<T, E>(error: E): RemoteData<T, E> {
    return new RemoteData<T, E>(new Failure<E>(error));
  }

  /**
   * Construct a new Remote Data that is loading
   */
  public static loading<T, E>(): RemoteData<T, E> {
    return new RemoteData<T, E>(new Loading());
  }

  /**
   * Construct a new Remote Data that was not asked for yet
   */
  public static notAsked<T, E>(): RemoteData<T, E> {
    return new RemoteData<T, E>(new NotAsked());
  }
  private state: RemoteDataState;

  private constructor(state: RemoteDataState) {
    this.state = state;
  }

  public match({
    successFunc,
    failureFunc,
    loadingFunc,
    notAskedFunc
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
    throw new Error('Unmatched case in RemoteData');
  }

  public map<R>(func: (value: T) => R): RemoteData<R, E> {
    return this.andThen<R>(val => RemoteData.success<R, E>(func(val)));
  }

  public andThen<R>(func: (value: T) => RemoteData<R, E>): RemoteData<R, E> {
    // tslint:disable-next-line: no-angle-bracket-type-assertion
    return this.match(<IRemoteDataMatchCases<T, E>>{
      successFunc: func,
      failureFunc: error => RemoteData.failure<T, E>(error),
      loadingFunc: () => RemoteData.loading<T, E>(),
      notAskedFunc: () => RemoteData.notAsked<T, E>()
    });
  }

  public withDefault(defaultValue: T): T {
    return this.hasData() ? this.getData() : defaultValue;
  }

  // checkers
  public isSuccess = () => this.state.isSuccess;
  public isFailure = () => this.state.isFailure;
  public isLoading = () => this.state.isLoading;
  public isNotAsked = () => this.state.isNotAsked;
  public hasData = () => this.isSuccess();
  public hasError = () => this.isFailure();

  // very "unmonadic" accessors
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
