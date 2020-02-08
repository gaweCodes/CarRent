import { TaggedUnion3State } from './TaggedUnion3State';

export class First<T1> extends TaggedUnion3State {
  public readonly isFirst: boolean = true;
  public readonly firstState: T1;

  constructor(firstState: T1) {
    super();
    this.firstState = firstState;
  }
}
