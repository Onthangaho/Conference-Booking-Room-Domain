import { Component } from "react";

class AppErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError() {
    return { hasError: true };
  }

  componentDidCatch(error) {
    console.error("Application error boundary caught an error", error);
  }

  handleReset = () => {
    this.setState({ hasError: false });
  };

  render() {
    if (this.state.hasError) {
      return (
        <main className="container">
          <section className="error-panel" role="alert">
            <h2>Something went wrong</h2>
            <p>The page failed to render correctly. Reset the view and try again.</p>
            <button type="button" className="btn primary" onClick={this.handleReset}>
              Reset view
            </button>
          </section>
        </main>
      );
    }

    return this.props.children;
  }
}

export default AppErrorBoundary;