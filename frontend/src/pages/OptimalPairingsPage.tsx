import { IconButton, Stack, CircularProgress, Alert, Typography, Box, Paper } from "@mui/material";
import { useNavigate } from "react-router-dom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useGetOptimalPairings } from "../features/pairings/hooks/usePairing";

export default function OptimalPairingsPage() {
    const navigate = useNavigate();
    const { data: optimalPairings, isLoading, isError } = useGetOptimalPairings();
    return (
        <Box
            sx={{
                maxWidth: 800, // adjust this to your desired width
                width: '100%',
                alignSelf: 'flex-start', // stick to top instead of center
            }}
        >
            <Stack spacing={2} sx={{ padding: 2, marginTop: 4 }}>
                <IconButton onClick={() => navigate("/")} aria-label="back" sx={{ alignSelf: 'flex-start' }}>
                    <ArrowBackIcon />
                </IconButton>
                <Typography variant="h3">Optimal Pairings</Typography>
                <Typography variant="body1">
                    Below are the scientifically proven most compatible couples based on a hollistic meta analysis of all current data combined with all sorts of whacky, futuristic algorithms and a dash agentic orchestration.
                </Typography>
                {isLoading && <CircularProgress />}
                {isError && <Alert severity="error">Error fetching optimal pairings</Alert>}
                {optimalPairings && (
                    <Stack spacing={2}>
                        {optimalPairings.map((pairing, index) => (
                            <Paper key={index} sx={{ padding: 2, textAlign: 'center' }} elevation={9}>
                                <Typography variant="h4">{pairing.firstMemberName} & {pairing.secondMemberName}</Typography>
                            </Paper>
                        ))}
                        <Typography variant="body1">
                            The above are in no particular order, and were chosen to maximise the sum of compatibility score of pairs in the group. This may mean that some people aren't in their most compatible pair, but hopefully their love of and unshaking devotion towards the group mean they're not too bummed out by that.
                        </Typography>
                        <Typography variant="body1">
                            Also, if there is an odd number of people, one person will not be in a pair. That is not to say that person isn't compatible with other people in the group, in fact, they may be in pairs that are more compatible than the ones in the above set, but unfortunately that's just how the algorithm works. If you are that person, we suggest hitting up one of these lovely couples to see if you can form a throuple.
                        </Typography>
                    </Stack>
                )}
            </Stack>
        </Box>
    );
}