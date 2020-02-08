import { RemoteDataState } from './RemoteDataState';

export class NotCreated<T> extends RemoteDataState {
  public readonly isNotCreated: boolean = true;
  public readonly data!: T;

  constructor(data: T) {
    super();
    this.data = data;
  }
}
