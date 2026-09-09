export interface ThemeVariableEntry {
    description: string;   // e.g., "backgroundColor", "textColor"
    color: string;         // e.g., "#ffffff"
}

export interface ThemeCacheEntry {
    themeId: number;
    variables: ThemeVariableEntry[];
}
