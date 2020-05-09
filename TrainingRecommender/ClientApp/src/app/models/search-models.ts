export class PaginationBase {
  page = 0;
  pageSize = 10;
}

export class SearchTrainings extends PaginationBase {
  muscle: number;
  my: boolean;
}
