import { IMatchCases } from './IMatchCases';

export interface IMonad<T> extends IFunctor<T> {
  /**
   * Match over all cases in the Monad
   *
   * @param cases An object representing all functions for all cases
   */
  match(cases: IMatchCases): any;

  /**
   * Apply the provided function to the wrapped data and unwrap the result
   *
   * @param func The function to apply
   */
  andThen<R>(func: (value: T) => IMonad<R>): IMonad<R>;

  /**
   * Returns the wrapped data if present or the defaultValue if not
   *
   * @param defaultValue The defaultValue to return
   */
  withDefault(defaultValue: T): T;
}

interface IFunctor<T> {
  /**
   * Map the provided function to the wrapped data.
   *
   * @param func The function to map
   */
  map<R>(func: (value: T) => R): IFunctor<R>;
}
