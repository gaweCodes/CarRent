import { TaggedUnion3State } from './TaggedUnion3State';

export class Third<T3> extends TaggedUnion3State {
  public readonly isThird: boolean = true;
  public readonly thirdState: T3;

  constructor(thirdState: T3) {
    super();
    this.thirdState = thirdState;
  }
}
