import { IMatchCases } from './IMatchCases';

export interface ITaggedUnion3<T1, T2, T3> {
  match(cases: IMatchCases): any;

  mapFirst<R>(func: (value: T1) => R): ITaggedUnion3<R, T2, T3>;
  mapSecond<R>(func: (value: T2) => R): ITaggedUnion3<T1, R, T3>;
  mapThird<R>(func: (value: T3) => R): ITaggedUnion3<T1, T2, R>;

  flatMapFirst<R>(func: (value: T1) => ITaggedUnion3<R, T2, T3>): ITaggedUnion3<R, T2, T3>;
  flatMapSecond<R>(func: (value: T2) => ITaggedUnion3<T1, R, T3>): ITaggedUnion3<T1, R, T3>;
  flatMapThird<R>(func: (value: T3) => ITaggedUnion3<T1, T2, R>): ITaggedUnion3<T1, T2, R>;
}
