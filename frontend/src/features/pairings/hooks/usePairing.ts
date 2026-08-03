import type { RatePairingRequest, GetRandomPairingResponse } from "../types/types";
import { api } from "../../../lib/api";
import { useMutation, useQuery } from "@tanstack/react-query";
import { ratePairing } from "../api/pairing";

export function useGetRandomPairing() {
    return useQuery<GetRandomPairingResponse>({
        queryKey: ["pairing", "random"],
        queryFn: async () => {
            const response = await api.get<GetRandomPairingResponse>("/pairing/random");
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
