export interface IErrorWrapper<E> {
  /**
   * Check if RemoteData is in a failure carrying state.
   */
  hasError(): boolean;
  /**
   * Get a reference to the error in a Failure Remote Data
   *
   * Very unmonadic, but great for reactive binding in Vue
   */
  getError(): E;
}
