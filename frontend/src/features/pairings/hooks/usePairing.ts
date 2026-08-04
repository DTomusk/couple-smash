import type { RatePairingRequest, GetRandomPairingResponse } from "../types/types";
import { useMutation, useQuery } from "@tanstack/react-query";
import { ratePairing, getRandomPairing, getOptimalPairings } from "../api/pairing";

export function useGetRandomPairing() {
    return useQuery<GetRandomPairingResponse>({
        queryKey: ["pairing", "random"],
        queryFn: async () => {
            const response = await getRandomPairing();
            return response;
        }
    });
}

export function useRatePairingMutation() {
    return useMutation({
        mutationFn: async (request: RatePairingRequest) => {
            return ratePairing(request);
        }
    });
}

export function useGetOptimalPairings() {
    return useQuery<GetRandomPairingResponse[]>({
        queryKey: ["pairing", "optimal"],
        queryFn: async () => {
            const response = await getOptimalPairings();
            return response;
        }
    });
}