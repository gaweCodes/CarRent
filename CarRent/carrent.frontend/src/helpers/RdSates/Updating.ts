import { Success } from './Success';

export class Updating<T> extends Success<T> {
  public readonly isUpdating: boolean = true;

  constructor(data: T) {
    super(data);
  }
}
