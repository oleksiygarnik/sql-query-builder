export class Filter {
  column: string;
  value: string;

  constructor(column: string, value: string) {
    this.column = column;
    this.value = value;
  }
}
