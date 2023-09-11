import { render, screen, waitFor, fireEvent } from '@testing-library/react'
import App from './App'
import axios from 'axios'

jest.mock('axios')

describe('App component', () => {
  const API_URL = `${process.env.REACT_APP_BASE_URL}/v1/api/todoitems`
  const todoItems = [
    { id: 1, description: 'Test Desc 1', isComplete: false },
    { id: 2, description: 'Test Desc 2', isComplete: false },
  ]

  beforeEach(() => {
    axios.get.mockReset()
  })

  it('renders with empty list', async () => {
    axios.get.mockResolvedValueOnce({
      data: [],
    })

    render(<App />)
    await waitFor(() => {
      expect(screen.getByText('Showing 0 Item(s)')).toBeInTheDocument()
    })
  })

  it('renders with todo list', async () => {
    axios.get.mockResolvedValueOnce({
      data: todoItems,
    })

    render(<App />)
    await waitFor(() => {
      expect(screen.getByText('Test Desc 1')).toBeInTheDocument()
      expect(screen.getByText('Test Desc 2')).toBeInTheDocument()
      expect(screen.getAllByRole('button', { name: 'Mark as completed' }).length).toBe(2)
      expect(screen.getByText('Showing 2 Item(s)')).toBeInTheDocument()
    })
  })

  it('renders with too list and mark an item as completed', async () => {
    axios.get.mockResolvedValueOnce({
      data: todoItems,
    })

    render(<App />)
    let markAsCompleteButton
    await waitFor(() => {
      markAsCompleteButton = screen.getAllByRole('button', { name: 'Mark as completed' })[0]
    })

    expect(screen.getByText('Showing 2 Item(s)')).toBeInTheDocument()

    fireEvent.click(markAsCompleteButton)

    await waitFor(() => {
      expect(screen.getByText('Showing 1 Item(s)')).toBeInTheDocument()
    })
  })
})
