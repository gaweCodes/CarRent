import { RemoteDataState } from './RemoteDataState';

export class Failure<E> extends RemoteDataState {
  public readonly isFailure: boolean = true;
  public readonly error!: E;

  constructor(error: E) {
    super();
    this.error = error;
  }
}
