import { TaggedUnion2State } from './TaggedUnion2State';

export class Second<T2> extends TaggedUnion2State {
  public readonly isSecond: boolean = true;
  public readonly secondState: T2;

  constructor(secondState: T2) {
    super();
    this.secondState = secondState;
  }
}
