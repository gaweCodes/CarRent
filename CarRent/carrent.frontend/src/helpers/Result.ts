// tslint:disable: max-classes-per-file
// tslint:disable: member-ordering
import { IDataWrapper } from './IDataWrapper';
import { IErrorWrapper } from './IErrorWrapper';
import { IMatchCases } from './IMatchCases';
import { IMonad } from './IMonad';

abstract class ResultState {
  public readonly isOk: boolean = false;
  public readonly isErr: boolean = false;
}

class Ok<T> extends ResultState {
  public readonly isOk: boolean = true;
  public readonly data: T;

  constructor(data: T) {
    super();
    this.data = data;
  }
}

class Err<E> extends ResultState {
  public readonly isErr: boolean = true;
  public readonly error: E;

  constructor(error: E) {
    super();
    this.error = error;
  }
}

interface IResultMatchCases<T, E> extends IMatchCases {
  okFunc: (data: T) => any;
  errFunc: (error: E) => any;
}

export class Result<T, E> implements IMonad<T>, IDataWrapper<T>, IErrorWrapper<E> {
  private state: ResultState;

  private constructor(state: ResultState) {
    this.state = state;
  }

  public match({ okFunc, errFunc }: IResultMatchCases<T, E>): any {
    return this.state.isOk === true
      ? okFunc((this.state as Ok<T>).data)
      : errFunc((this.state as Err<E>).error);
  }

  public map<R>(func: (value: T) => R): Result<R, E> {
    return this.andThen<R>(val => Result.ok<R, E>(func(val)));
  }

  public andThen<R>(func: (value: T) => Result<R, E>): Result<R, E> {
    return this.state.isOk === true
      ? func((this.state as Ok<T>).data)
      : Result.err<R, E>((this.state as Err<E>).error);
  }

  public withDefault(defaultValue: T): T {
    return this.state.isOk === true ? (this.state as Ok<T>).data : defaultValue;
  }

  // checkers
  public isErr = () => this.state.isErr === true;
  public isOk = () => this.state.isOk === true;
  public hasData = () => this.isOk();
  public hasError = () => this.isErr();

  // very "unmonadic" accessors
  public getData(): T {
    if (this.state.isOk) {
      return (this.state as Ok<T>).data;
    }
    throw new Error('Result is not in Ok state');
  }

  public getError(): E {
    if (this.state.isErr) {
      return (this.state as Err<E>).error;
    }
    throw new Error('Result is not in Err state');
  }

  // static constructors
  public static ok<T, E>(data: T) {
    return new Result<T, E>(new Ok<T>(data));
  }
  public static err<T, E>(error: E) {
    return new Result<T, E>(new Err<E>(error!));
  }
}
