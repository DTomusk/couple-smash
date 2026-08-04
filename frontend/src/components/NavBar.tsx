import { Button, Typography } from '@mui/material';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import { useNavigate } from 'react-router-dom';

export default function NavBar() {
    const navigate = useNavigate();
    return (
        <>
            <AppBar position="fixed" color="primary">
                <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
                    <Typography variant="h5">💍Couple Smash💑</Typography>
                    <Button variant="text" color="inherit" onClick={() => navigate('/optimal-pairings')}>
                        Optimal Pairings
                    </Button>
                </Toolbar>
            </AppBar>
        </>
    )
}