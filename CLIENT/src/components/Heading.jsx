function Heading({ title, text }) {
    return <h1 className="heading">{title || text}</h1>;
}

export default Heading;