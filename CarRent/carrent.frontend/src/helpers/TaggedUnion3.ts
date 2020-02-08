import { IMatchCases } from './IMatchCases';
import { ITaggedUnion3 } from './ITaggedUnion3';
import { TaggedUnion3State } from './TaggedUnion/TaggedUnion3State';
import { Third } from './TaggedUnion/Third';
import { Second } from './TaggedUnion/Second';
import { First } from './TaggedUnion/First';

interface IResultMatchCases<T1, T2, T3> extends IMatchCases {
  firstFunc: (firstState: T1) => any;
  secondFunc: (secondState: T2) => any;
  thirdFunc: (thirdState: T3) => any;
}

export class TaggedUnion3<T1, T2, T3> implements ITaggedUnion3<T1, T2, T3> {
  public static first<T1, T2, T3>(firstState: T1) {
    return new TaggedUnion3<T1, T2, T3>(new First<T1>(firstState));
  }
  public static second<T1, T2, T3>(secondState: T2) {
    return new TaggedUnion3<T1, T2, T3>(new Second<T2>(secondState));
  }
  public static third<T1, T2, T3>(thirdState: T3) {
    return new TaggedUnion3<T1, T2, T3>(new Third<T3>(thirdState));
  }

  private state: TaggedUnion3State;

  private constructor(state: TaggedUnion3State) {
    this.state = state;
  }

  public match({ firstFunc, secondFunc, thirdFunc }: IResultMatchCases<T1, T2, T3>): any {
    return this.state.isFirst === true
      ? firstFunc((this.state as First<T1>).firstState)
      : this.state.isSecond === true
      ? secondFunc((this.state as Second<T2>).secondState)
      : thirdFunc((this.state as Third<T3>).thirdState);
  }

  public mapFirst<R>(func: (value: T1) => R): TaggedUnion3<R, T2, T3> {
    return this.flatMapFirst<R>(val => TaggedUnion3.first<R, T2, T3>(func(val)));
  }

  public flatMapFirst<R>(func: (value: T1) => TaggedUnion3<R, T2, T3>): TaggedUnion3<R, T2, T3> {
    return this.state.isFirst === true
      ? func((this.state as First<T1>).firstState)
      : this.state.isSecond === true
      ? TaggedUnion3.second<R, T2, T3>((this.state as Second<T2>).secondState)
      : TaggedUnion3.third<R, T2, T3>((this.state as Third<T3>).thirdState);
  }

  public mapSecond<R>(func: (value: T2) => R): TaggedUnion3<T1, R, T3> {
    return this.flatMapSecond<R>(val => TaggedUnion3.second<T1, R, T3>(func(val)));
  }

  public flatMapSecond<R>(func: (value: T2) => TaggedUnion3<T1, R, T3>): TaggedUnion3<T1, R, T3> {
    return this.state.isSecond === true
      ? func((this.state as Second<T2>).secondState)
      : this.state.isThird === true
      ? TaggedUnion3.third<T1, R, T3>((this.state as Third<T3>).thirdState)
      : TaggedUnion3.first<T1, R, T3>((this.state as First<T1>).firstState);
  }

  public mapThird<R>(func: (value: T3) => R): TaggedUnion3<T1, T2, R> {
    return this.flatMapThird<R>(val => TaggedUnion3.third<T1, T2, R>(func(val)));
  }

  public flatMapThird<R>(func: (value: T3) => TaggedUnion3<T1, T2, R>): TaggedUnion3<T1, T2, R> {
    return this.state.isThird === true
      ? func((this.state as Third<T3>).thirdState)
      : this.state.isFirst === true
      ? TaggedUnion3.first<T1, T2, R>((this.state as First<T1>).firstState)
      : TaggedUnion3.second<T1, T2, R>((this.state as Second<T2>).secondState);
  }

  public isSecond = () => this.state.isSecond === true;
  public isFirst = () => this.state.isFirst === true;
  public isThird = () => this.state.isThird === true;

  public getFirst(): T1 {
    if (this.state.isFirst) {
      return (this.state as First<T1>).firstState;
    }
    throw new Error('TaggedUnion3 is not in First state');
  }

  public getSecond(): T2 {
    if (this.state.isSecond) {
      return (this.state as Second<T2>).secondState;
    }
    throw new Error('TaggedUnion3 is not in Second state');
  }

  public getThird(): T3 {
    if (this.state.isThird) {
      return (this.state as Third<T3>).thirdState;
    }
    throw new Error('TaggedUnion3 is not in Third state');
  }
}
