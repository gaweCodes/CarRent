import { TaggedUnion2State } from './TaggedUnion2State';

export class First<T1> extends TaggedUnion2State {
  public readonly isFirst: boolean = true;
  public readonly firstState: T1;

  constructor(firstState: T1) {
    super();
    this.firstState = firstState;
  }
}
