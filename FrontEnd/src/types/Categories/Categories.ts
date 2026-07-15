
export interface CategoryBase {
    id: number;
    userId: number;
    name: string | null;
    isActive: boolean;
    sortOrder: number;

}

export interface Category extends CategoryBase{
    
}