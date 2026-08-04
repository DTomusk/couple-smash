import { IconButton, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";

export default function OptimalPairingsPage() {
    const navigate = useNavigate();
    return (
        <Stack spacing={2} sx={{ padding: 2, marginTop: 12 }}>
            <IconButton onClick={() => navigate("/")} aria-label="back">
                <ArrowBackIcon />
            </IconButton>
        </Stack>
    );
}