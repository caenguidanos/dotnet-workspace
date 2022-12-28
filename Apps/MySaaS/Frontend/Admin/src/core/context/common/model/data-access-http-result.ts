export interface HttpDataAccessResult<K> {
    status: number | null;
    payload: K | null;
}
