import { useEffect, useState } from "react";
import type { FileEntryModel } from "../types";
import { getList } from "../services/storage-service";

export function useStorage() {

    const dummydata: FileEntryModel[] = [
        { id: "1", name: "Laptop", description: "Electronics", uploadedAt: "2024-06-01", size: 999.99 },
        { id: "2", name: "Coffee Maker", description: "Home Appliances", uploadedAt: "2024-06-02", size: 49.99 },
        { id: "3", name: "Desk Chair", description: "Furniture", uploadedAt: "2024-06-03", size: 150.0 },
        { id: "4", name: "Smartphone", description: "Electronics", uploadedAt: "2024-06-04", size: 799.99 },
        { id: "5", name: "Headphones", description: "Accessories", uploadedAt: "2024-06-05", size: 199.99 },
    ]

    const [data, setData] = useState<FileEntryModel[]>(dummydata);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);

    const fetchData = async () => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await getList();
            setData(response.data);
        }
        catch (error) {
            setError(error instanceof Error ? error : new Error("Unknown error"));
        }
        finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    return {
        data,
        isLoading,
        error,
        refresh: fetchData
    };
}