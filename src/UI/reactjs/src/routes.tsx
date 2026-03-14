import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import { StoragePage } from './features/Storage'
import { Toaster } from './components/ui/toaster'

function App() {
  return (
    <>
      <Router>
        <Routes>
          <Route path="/" element={<StoragePage />} />
        </Routes>
      </Router>

      <Toaster />
    </>
  )
}

export default App
