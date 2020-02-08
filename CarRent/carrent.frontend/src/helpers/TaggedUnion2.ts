import { IMatchCases } from './IMatchCases';
import { ITaggedUnion2 } from './ITaggedUnion2';

abstract class TaggedUnion2State {
  public readonly isFirst: boolean = false;
  public readonly isSecond: boolean = false;
}

class First<T1> extends TaggedUnion2State {
  public readonly isFirst: boolean = true;
  public readonly firstState: T1;

  constructor(firstState: T1) {
    super();
    this.firstState = firstState;
  }
}

class Second<T2> extends TaggedUnion2State {
  public readonly isSecond: boolean = true;
  public readonly secondState: T2;

  constructor(secondState: T2) {
    super();
    this.secondState = secondState;
  }
}

interface IResultMatchCases<T1, T2> extends IMatchCases {
  firstFunc: (firstState: T1) => any;
  secondFunc: (secondState: T2) => any;
}

export class TaggedUnion2<T1, T2> implements ITaggedUnion2<T1, T2> {
  private state: TaggedUnion2State;

  private constructor(state: TaggedUnion2State) {
    this.state = state;
  }

  public match({ firstFunc, secondFunc }: IResultMatchCases<T1, T2>): any {
    return this.state.isFirst === true
      ? firstFunc((this.state as First<T1>).firstState)
      : secondFunc((this.state as Second<T2>).secondState);
  }

  public mapFirst<R>(func: (value: T1) => R): TaggedUnion2<R, T2> {
    return this.flatMapFirst<R>(val => TaggedUnion2.first<R, T2>(func(val)));
  }

  public flatMapFirst<R>(func: (value: T1) => TaggedUnion2<R, T2>): TaggedUnion2<R, T2> {
    return this.state.isFirst === true
      ? func((this.state as First<T1>).firstState)
      : TaggedUnion2.second<R, T2>((this.state as Second<T2>).secondState);
  }

  public mapSecond<R>(func: (value: T2) => R): TaggedUnion2<T1, R> {
    return this.flatMapSecond<R>(val => TaggedUnion2.second<T1, R>(func(val)));
  }

  public flatMapSecond<R>(func: (value: T2) => TaggedUnion2<T1, R>): TaggedUnion2<T1, R> {
    return this.state.isFirst === true
      ? TaggedUnion2.first<T1, R>((this.state as First<T1>).firstState)
      : func((this.state as Second<T2>).secondState);
  }

  // checkers
  public isSecond = () => this.state.isSecond === true;
  public isFirst = () => this.state.isFirst === true;

  // very "unmonadic" accessors
  public getFirst(): T1 {
    if (this.state.isFirst) {
      return (this.state as First<T1>).firstState;
    }
    throw new Error('TaggedUnion2 is not in First state');
  }

  public getSecond(): T2 {
    if (this.state.isSecond) {
      return (this.state as Second<T2>).secondState;
    }
    throw new Error('TaggedUnion2 is not in Second state');
  }

  // static constructors
  public static first<T1, T2>(firstState: T1) {
    return new TaggedUnion2<T1, T2>(new First<T1>(firstState));
  }
  public static second<T1, T2>(secondState: T2) {
    return new TaggedUnion2<T1, T2>(new Second<T2>(secondState));
  }
}
