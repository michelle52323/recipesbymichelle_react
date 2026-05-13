export interface FractionDecimal {
    id: number;
    fraction: string | null;
    decimal: number | null;
    primary: boolean;
}

export interface MeasurementUnit {
    id: number;
    description: string | null;
    abbreviation: string | null;
    system: number | null;   // 1 = Metric, 2 = Imperial, null = universal
    plural: string | null;
}
