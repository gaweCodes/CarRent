import { Success } from './Success';

export class UpdateFailure<T, E> extends Success<T> {
  public readonly isUpdateFailure = true;
  public readonly error!: E;

  constructor(data: T, error: E) {
    super(data);
    this.error = error;
  }
}
