import { TaggedUnion3State } from './TaggedUnion3State';

export class Second<T2> extends TaggedUnion3State {
  public readonly isSecond: boolean = true;
  public readonly secondState: T2;

  constructor(secondState: T2) {
    super();
    this.secondState = secondState;
  }
}
