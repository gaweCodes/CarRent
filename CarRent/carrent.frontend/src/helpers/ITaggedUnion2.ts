import { IMatchCases } from './IMatchCases';

export interface ITaggedUnion2<T1, T2> {
  match(cases: IMatchCases): any;

  mapFirst<R>(func: (value: T1) => R): ITaggedUnion2<R, T2>;
  mapSecond<R>(func: (value: T2) => R): ITaggedUnion2<T1, R>;

  flatMapFirst<R>(func: (value: T1) => ITaggedUnion2<R, T2>): ITaggedUnion2<R, T2>;
  flatMapSecond<R>(func: (value: T2) => ITaggedUnion2<T1, R>): ITaggedUnion2<T1, R>;
}
