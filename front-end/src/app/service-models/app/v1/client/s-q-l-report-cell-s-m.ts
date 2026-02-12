import { CellDataType } from '../../enums/cell-data-type.enum';

export class SQLReportCellSM {
    cellColumnIndex!: number;
    cellColumnName!: string;
    cellDataType!: CellDataType;
    cellValue!: Object;
}
