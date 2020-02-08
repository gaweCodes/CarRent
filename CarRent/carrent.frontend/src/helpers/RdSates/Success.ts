import { RemoteDataState } from './RemoteDataState';

export class Success<T> extends RemoteDataState {
  public readonly isSuccess: boolean = true;
  public readonly data!: T;

  constructor(data: T) {
    super();
    this.data = data;
  }
}
