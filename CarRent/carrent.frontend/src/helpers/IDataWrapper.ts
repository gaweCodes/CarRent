export interface IDataWrapper<T> {
  /**
   * Check if RemoteData is in a data carrying state.
   */
  hasData(): boolean;

  /**
   * Get a reference to the data in a Success Remote Data
   *
   * Very unmonadic, but great for reactive binding in Vue
   */
  getData(): T;
}
