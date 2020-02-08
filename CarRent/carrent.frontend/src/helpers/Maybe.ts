import { IDataWrapper } from './IDataWrapper';
import { IMatchCases } from './IMatchCases';
import { IMonad } from './IMonad';

abstract class MaybeState {
  public readonly isJust: boolean = false;
  public readonly isNone: boolean = false;
}

// tslint:disable-next-line: max-classes-per-file
class Just<T> extends MaybeState {
  public readonly isJust: boolean = true;
  public readonly data: T;

  constructor(data: T) {
    super();
    this.data = data;
  }
}

// tslint:disable-next-line: max-classes-per-file
class None extends MaybeState {
  public readonly isNone: boolean = true;
}

interface IMaybeMatchCases<T> extends IMatchCases {
  justFunc: (data: T) => any;
  noneFunc: () => any;
}

// tslint:disable-next-line: max-classes-per-file
export class Maybe<T> implements IMonad<T>, IDataWrapper<T> {
  // static constructors
  // tslint:disable-next-line: member-ordering
  public static just<T>(data: T) {
    return new Maybe<T>(new Just<T>(data));
  }
  public static none<T>() {
    return new Maybe<T>(new None());
  }
  private state: MaybeState;

  private constructor(state: MaybeState) {
    this.state = state;
  }

  public match({ justFunc, noneFunc }: IMaybeMatchCases<T>): any {
    return this.state.isJust === true ? justFunc((this.state as Just<T>).data) : noneFunc();
  }

  public map<R>(func: (value: T) => R): Maybe<R> {
    return this.andThen<R>(val => Maybe.just<R>(func(val)));
  }

  public andThen<R>(func: (value: T) => Maybe<R>): Maybe<R> {
    return this.state.isJust === true ? func((this.state as Just<T>).data) : Maybe.none<R>();
  }

  public withDefault(defaultValue: T): T {
    return this.state.isJust === true ? (this.state as Just<T>).data : defaultValue;
  }

  // checkers
  public isJust = () => this.state.isJust === true;
  public isNone = () => this.state.isNone === true;
  public hasData = () => this.isJust();

  // very "unmonadic" accessor
  public getData(): T {
    if (this.state.isJust) {
      return (this.state as Just<T>).data;
    }
    throw new Error('Maybe is not in Just<T> state');
  }
}
