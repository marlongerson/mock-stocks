import { faGithub } from "@fortawesome/free-brands-svg-icons"
import { faCircleNotch } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { Line } from "react-chartjs-2"
import axios from 'axios'
import 'chart.js/auto'
import { useState } from "react"

interface PriceHistoryResponse {
  symbol: string,
  history: number[],
}

function App() {
  const [errorText, setErrorText] = useState('')
  const [symbol, setSymbol] = useState('')
  const [loading, setLoading] = useState(false)
  const [label, setLabel] = useState('')
  const [history, setHistory] = useState<number[]>([])

  async function getPriceHistory() {
    if (symbol.length == 0) {
      return
    }
    // Add an artificial delay of 500 ms to prevent animation flashing.
    const minimumDelay = 500
    const start = Date.now()
    setLoading(true)
    try {
      const res = await axios.get<PriceHistoryResponse>(`${import.meta.env.VITE_BACKEND_URL}/stocks/${symbol}`)
      // Add an artificial delay if the request completed faster.
      const end = Date.now()
      if (end - start < minimumDelay) {
        await new Promise(resolve => setTimeout(resolve, minimumDelay - (end - start)))
      }      
      // Update the chart.
      setHistory(res.data.history)
      setLabel(symbol)
    } catch {
      setErrorText('Failed to fetch price history. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  async function onInputKeyDown(event: React.KeyboardEvent<HTMLInputElement>) {
    if (event.key == 'Enter') {
      await getPriceHistory()
    }
  }

  function generateDates(count: number): string[] {
    return Array(count).fill(new Date).map(date => new Date(date.setDate(date.getDate() - 1)).toDateString()).reverse()
  }

  return (
    <>
      <div className="px-8 p-2 border-b border-gray-200 bg-gray-100">
        <div className="flex justify-between items-center">
          <h1 className="font-bold text-xl">MockStocks</h1>
          <FontAwesomeIcon className="text-4xl" icon={faGithub}/>
        </div>
      </div>
      <div className="px-8 py-4">
        <div className="px-8 py-4">
          <div>
            <input
              type="text"
              className="px-4 py-2 border border-r-0 rounded-l-sm outline-none"
              placeholder="Symbol"
              value={symbol}
              onKeyDown={e => onInputKeyDown(e)}
              onChange={e => setSymbol(e.target.value)}
            />
            <button
              className="px-4 py-2 bg-cyan-700 text-white rounded-r-sm w-20"
              onClick={getPriceHistory}
              disabled={loading}
            >
              {loading ? <FontAwesomeIcon icon={faCircleNotch} spin className="text-xl"/> : <>Fetch</>}
            </button>
          </div>
          <div>
            {errorText.length > 0 ? (
              <><span className="text-red-500">{errorText}</span></>
            ) : (
              <></>
            )}
          </div>
        </div>
        <div style={{maxWidth: '1000px', height: '1000px'}}>
          <Line
            options={
              {
                plugins: {
                  title: {
                    display: true,
                    text: "History"
                  }
                },
                scales: {
                  x: {
                    title: {
                      display: true,
                      text: 'Date'
                    }
                  },
                  y: {
                    title: {
                      display: true,
                      text: 'Price'
                    }
                  }
                }
              }
            }
            datasetIdKey='0'
            data={{
              labels: generateDates(history.length),
              datasets: [
                {
                  label: label.length == 0 ? 'No symbol selected' : `$${label.toUpperCase()}`,
                  data: history,
                },
              ],
            }}
          />
        </div>
      </div>
    </>
  )
}

export default App
